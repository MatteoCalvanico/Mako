using Mako.Services.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mako.Services.Shared
{
    public class AddOrUpdateLicencesCommand
    {
        public int Id { get; set; }
        public LicenceTypes Types { get; set; }
    }
    
    public partial class SharedService
    {
        public async Task<int> Handle(AddOrUpdateLicencesCommand cmd)
        {
            var licence = await _dbContext.Licences
                .Where(x => x.Id == cmd.Id)
                .FirstOrDefaultAsync();
            if (licence == null)
            {
                licence = new Licence
                {
                    Id = cmd.Id,
                    Types = cmd.Types,
                };
                _dbContext.Licences.Add(licence);
            }
            
            licence.Id = cmd.Id;

            await _dbContext.SaveChangesAsync();

            return licence.Id;
        }
    }
}
