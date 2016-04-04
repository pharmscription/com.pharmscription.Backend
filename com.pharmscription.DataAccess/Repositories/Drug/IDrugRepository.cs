<<<<<<< 2ab2d6310285d0e9aa71f837768fb240d5a752c0
﻿using System.Collections.Generic;
=======
﻿
using System;
using System.Collections.Generic;
>>>>>>> Implemented Drug Entity, Added Interface for Drugrepository
using System.Threading.Tasks;
using com.pharmscription.DataAccess.SharedInterfaces;

namespace com.pharmscription.DataAccess.Repositories.Drug
{
    public interface IDrugRepository : IRepository<Entities.DrugEntity.Drug>
    {
        Task<List<Entities.DrugEntity.Drug>> SearchByName(string name);
    }
}
