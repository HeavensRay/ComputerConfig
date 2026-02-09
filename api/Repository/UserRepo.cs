// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using api.Data;
// using api.Interfaces;
// using api.Entities;
// using Microsoft.AspNetCore.Http.HttpResults;
// using Microsoft.EntityFrameworkCore;
// using api.Dto;

// DEPRECATED
//            return await _context.Users.Include(c => c.Configs).ToListAsync();

// namespace api.Repository
// {
//     public class UserRepo : IUserRepo
//     {
//         private readonly AppDbContext _context;
//         public UserRepo(AppDbContext context)
//         {
//             _context = context;
//         }

//         public async Task<User> CreateAsync(User UserEntity)
//         {
//              // adds a Entity to db
//             await _context.Users.AddAsync(UserEntity);
//             await _context.SaveChangesAsync();
//             return UserEntity;
//         }

//         public Task<bool> Exists(string userId)
//         {
//             return _context.Users.AnyAsync(u => u.Username == userId);
//         }

//         public async Task<List<User>> GetAllAsync()
//         {
//             // include is what makes list display possible
//            return await _context.Users.Include(c => c.Configs).ToListAsync();
//         }

//         public async Task<User?> GetByUsernameAsync(string username)
//         {
//             // find doesnt work w include
//             return await _context.Users.Include(c => c.Configs).FirstOrDefaultAsync(u => u.Username == username);
//         }
//     }
// }