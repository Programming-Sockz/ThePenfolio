using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThePenfolio.Server.Models;
using ThePenfolio.Server.Services;
using ThePenfolio.Shared.DTOs;
namespace ThePenfolio.Server.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ThePenfolioDbContext _context;
        private readonly IMapper _mapper;

        public UserController(ThePenfolioDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #region Get
        [HttpGet("users")]
        public async Task<ActionResult<List<UserDTO>>> GetUsers()
        {
            var users = await _context.Users.Where(x=>x.IsActive).Select(x => new UserDTO(){ Id = x.Id, UserName = x.UserName, DateJoined = x.DateJoined}).ToListAsync();
            
            return Ok(users);
        }

        [HttpGet("user/{id}")]
        public async Task<ActionResult<UserDTO>> GetUserById(Guid id)
        {
            var user = await _context.Users.Where(x=>x.IsActive && x.Id == id).Select(x => new UserDTO() { Id = x.Id, UserName = x.UserName, DateJoined = x.DateJoined }).FirstOrDefaultAsync();

            if(user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
        #endregion

        #region PUT
        [HttpPut]
        public async Task<ActionResult<UserDTO>> PutUser(UserDTO userDTO)
        {
            var user = await _context.Users.FindAsync(userDTO.Id);

            if (user == null)
            {
                return NotFound();
            }

            _mapper.From(userDTO).AdaptTo(user);

            await _context.SaveChangesAsync();

            return Ok(userDTO);
        }
        #endregion
        
        #region register/login
        [HttpPost("register")]
        public async Task<LoginResponseDTO> RegisterUser(RegisterDTO registerDTO)
        {
            if(_context.Users.Any(x=>x.Email == registerDTO.Email))
            {
                return new LoginResponseDTO() { Success = false, ErrorMessage = "Email already exists" };
            }
            
            if(_context.Users.Any(x=>x.UserName == registerDTO.UserName))
            {
                return new LoginResponseDTO() { Success = false, ErrorMessage = "Username already exists" };
            }

            var user = new User()
            {
                UserName = registerDTO.UserName,
                Email = registerDTO.Email,
                Password = registerDTO.Password,
                DateJoined = DateTime.Now,
                IsActive = true
            };

            _context.Users.Add(user);

            await _context.SaveChangesAsync();
            
            return new LoginResponseDTO() { Success = true, LoginStamp = Guid.NewGuid(), UserId = user.Id };
        }

        [HttpPost("login")]
        public async Task<LoginResponseDTO> LoginUser(LoginRequestDTO loginDTO)
        {
            if (!_context.Users.Any(x=>x.Email == loginDTO.Email))
            {
                return new LoginResponseDTO() { Success = false, ErrorMessage = "Email doesn't exist" };
            }
            
            var user = await _context.Users.FirstAsync(x => x.Email == loginDTO.Email);
                
            if(!user.IsActive)
            {
                return new LoginResponseDTO() { Success = false, ErrorMessage = "User is deactivated" };
            }
                
            if (user.Password == loginDTO.Password)
            {
                return new LoginResponseDTO() { Success = true, LoginStamp = Guid.NewGuid(), UserId = user.Id };
            }
                
            return new LoginResponseDTO() { Success = false, ErrorMessage = "Password is incorrect" };
        }
        #endregion

        #region (De)activate
        [HttpPut("deactivate/{id}")]
        public async Task<IActionResult> DeactivateUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            user.IsActive = false;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("activate/{id}")]
        public async Task<IActionResult> ActivateUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            user.IsActive = true;

            await _context.SaveChangesAsync();

            return Ok();
        }
        #endregion
    }
}
