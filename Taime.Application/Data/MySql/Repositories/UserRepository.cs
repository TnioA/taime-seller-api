﻿using Taime.Application.Data.MySql.Entities;
using Taime.Application.Utils.Data.MySql;

namespace Taime.Application.Data.MySql.Repositories
{
    public class UserRepository : MySqlRepositoryBase<UserEntity>
    {
        public UserRepository(MySqlContext mySqlContext) : base(mySqlContext) { }
    }
}