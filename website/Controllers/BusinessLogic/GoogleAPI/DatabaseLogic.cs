using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using website.Models;
using website.Models.databaseModels;
using website.Models.databaseModels.HelperModels;

namespace website.Controllers.BusinessLogic.GoogleReader
{
    public class DatabaseLogic
    {
        private DatabaseContext db;
        private IConfiguration configuration;

        public DatabaseLogic(IConfiguration configuration)
        {
            this.configuration = configuration;
            db = new DatabaseContext(configuration.GetConnectionString("UploadConnection"));
        }
        

        public async void LargeAdd(List<insertReady> entriesFromGoogleSheet)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {

                
                try
                {
                    db.ChangeTracker.AutoDetectChangesEnabled = false;
                    int count = 0;
                    foreach (var entry in entriesFromGoogleSheet)
                    {
                        ++count;

                        //Divide up the entry into the various tables and add them.

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
                    
                    db.ChangeTracker.AutoDetectChangesEnabled = true;
                    await db.SaveChangesAsync();
                }
            }
        }
    }
}
