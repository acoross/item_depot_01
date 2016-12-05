using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using bank.DbModel;

namespace bank.Contents
{   
    public class PersonalBank
    {
        readonly BankDbContext context;
        readonly long owner_id;

        public PersonalBank(BankDbContext context, long owner_id)
        {
            this.context = context;
            this.owner_id = owner_id;
        }

        bool UpdateStackable(TransactionData s)
        {
            if (s.amount_diff == 0)
                return true;
            
            var found = context.tb_stackable_items.SingleOrDefault(ss => ss.owner_id == owner_id && ss.type == s.item_type);
            if (found == null)
            {
                if (s.amount_diff > 0)
                {
                    context.tb_stackable_items.Add(new Stackable
                    {
                        owner_id = owner_id,
                        type = s.item_type,
                        amount = s.amount_diff
                    });
                }
                else
                {
                    return false;
                }
            }
            else
            {
                found.amount += s.amount_diff;
                if (found.amount > 0)
                {
                    context.tb_stackable_items.Update(found);
                }
                else if (found.amount == 0)
                {
                    context.tb_stackable_items.Remove(found);
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        bool UpdateNonS(TransactionData ns)
        {
            if (ns.amount_diff > 0)
            {
                context.tb_nonstackable_items.Add(new NonStackable
                {
                    owner_id = owner_id,
                    type = ns.item_type
                });
            }
            else
            {
                context.tb_nonstackable_items.Remove(new NonStackable
                {
                    owner_id = owner_id,
                    type = ns.item_type,
                    dbid = ns.dbid
                });
            }

            return true;
        }
        
        public bool ProcessTransaction(Transaction tran)
        {
            if (owner_id != tran.owner_id)
                return false;

            var jobDoneStack = new Stack<ITransactionalJob>();

            bool doCommit = false;
            context.Database.BeginTransaction();
            try
            {
                foreach (var item in tran.tran_list)
                {
                    if (item.job_type == TransactionJobType.Stackable)
                    {
                        if (!UpdateStackable(item))
                        {
                            return false;
                        }
                    }
                    else if (item.job_type == TransactionJobType.NonStackable)
                    {
                        if (!UpdateNonS(item))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        //todo - job
                    }
                }

                foreach (var job in tran.job_list)
                {
                    if (job.Do())
                    {
                        jobDoneStack.Push(job);
                    }
                    else
                    {
                        return false;
                    }
                }

                doCommit = true;
                return true;
            }
            finally
            {
                if (doCommit)
                {
                    context.SaveChanges();
                    context.Database.CommitTransaction();
                }
                else
                {
                    foreach (var done in jobDoneStack)
                    {
                        done.Rollback();
                    }

                    context.Database.RollbackTransaction();
                }
            }
        }
    }
}
