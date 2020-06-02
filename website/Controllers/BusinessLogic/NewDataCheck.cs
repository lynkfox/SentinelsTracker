using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using website.Models;
using website.Models.databaseModels;

namespace website.Controllers.BusinessLogic
{
    public class NewDataCheck
    {
        private DatabaseContext db;
        private IConfiguration configuration;



        public NewDataCheck(IConfiguration configuration)
        {
            this.configuration = configuration;
            db = new DatabaseContext(configuration.GetConnectionString("UploadConnection"));
        }

        public async void AddUpdateBoxSets(List<BoxSet> boxSets)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {

                List<BoxSet> boxSetsToBeUpdated = new List<BoxSet>();
                try
                {
                    db.ChangeTracker.AutoDetectChangesEnabled = false;
                    int count = 0;
                    foreach (var boxToCheck in boxSets)
                    {
                        ++count;

                        BoxSet inDatabaseBox = db.BoxSet.Where(x => x.Name == boxToCheck.Name).FirstOrDefault();

                        //add to the database if its not already there
                        if (inDatabaseBox is null)
                        {
                            db.BoxSet.Add(boxToCheck);
                        }

                        //if it's different, than add it to a list to be updated later (after adding all new entries)
                        else if (boxToCheck.CompareEquals(inDatabaseBox) is false)
                        {
                            boxToCheck.ID = inDatabaseBox.ID;
                            boxSetsToBeUpdated.Add(boxToCheck);
                        }

                        //Only save the database changes every 100 entries. Testing has shown this seems to be the most effecient for large updates.
                        if (count % 100 == 0)
                        {
                            await db.SaveChangesAsync();
                            db.Dispose();

                            db = new DatabaseContext(configuration.GetConnectionString("UploadConnection"));
                            db.ChangeTracker.AutoDetectChangesEnabled = false;
                        }

                    }

                    await db.SaveChangesAsync();
                }
                finally
                {
                    db = new DatabaseContext(configuration.GetConnectionString("UploadConnection"));
                    db.ChangeTracker.AutoDetectChangesEnabled = true;
                    db.UpdateRange(boxSetsToBeUpdated);
                    await db.SaveChangesAsync();
                }


                scope.Complete();
            }
        }

        private DatabaseContext saveEveryHundred(int count, DatabaseContext db)
        {
            throw new NotImplementedException();
        }
    }
}