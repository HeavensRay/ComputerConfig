// using System;
// using System.Collections.Generic;
// using System.Threading.Tasks;
// using api.Dto.User;
// using api.Entities;

// DEPRECATED

// namespace api.Mappers
// {
//     public static class UserMapper
//     {
//         public static GetUserDto ToGetDto(this User Entity)
//         {
//             return new GetUserDto
//             {
//                 Username = Entity.Username,
//                 // get comnfigs from model in db(w join i assume) turn to dto and send back
//                 Configs = Entity.Configs.Select(c => c.ToGetDto()).ToList()
//             };
//         }
//         public static User ToCreateEntity(this PostUserDto Dto)
//         {
//             return new User
//             {
//                 Username = Dto.Username,
//                 Password = Dto.Password
//             };
//         }
//     }
// }