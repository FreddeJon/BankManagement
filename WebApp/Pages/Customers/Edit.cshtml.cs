using Application.Features.Customer.Commands.EditCustomer;
using Application.Features.Customer.Query.GetCustomerByIdIncludeAccounts;

namespace WebApp.Pages.Customers
{
    public class EditModel : PageModel
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public EditModel(IMapper mapper, IMediator mediator)
        {
            _mapper = mapper;
            _mediator = mediator;
        }

        [BindProperty] public EditCustomerModel Customer { get; set; }
        public List<SelectListItem> Countries { get; set; }


        public async Task<IActionResult> OnGet(int id)
        {
            SetUpDropdowns();

            var response = await _mediator.Send(new GetCustomerQuery() { CustomerId = id });

            if (response.Status == Application.Responses.StatusCode.Error)
            {
                TempData["ErrorMessage"] = $"{response.StatusText}";
                return RedirectToPage("/PageNotFound");
            }
            Customer = _mapper.Map<EditCustomerModel>(response.Customer);

            return Page();
        }

        public async Task<IActionResult> OnPost(int id)
        {
            SetUpDropdowns();
            if (!ModelState.IsValid) return Page();

            var response = await _mediator.Send(new EditCustomerCommand() { Customer = _mapper.Map<CustomerDto>(Customer) });

            if (response.Status == Application.Responses.StatusCode.Error)
            {
                response.Errors?.ForEach(x => ModelState.AddModelError(x.PropertyName, x.ErrorMessage));


                return Page();
            }

            TempData["Message"] = $"Edited customer!";
            return Redirect($"/Customers/{id}");
        }

        public void SetUpDropdowns()
        {
            Countries = new List<SelectListItem>()
            {
                new()
                {
                    Disabled = true,
                    Selected = true,
                    Text = "Choose"
                },
                new() {Disabled = false, Text = "Sweden", Value = "SE"},
                new() {Disabled = false, Text = "Norway", Value = "NO"},
                new() {Disabled = false, Text = "Finland", Value = "FI"}
            };
        }


        public class EditCustomerModel
        {
            [Required]
            public int Id { get; set; }

            [Required(ErrorMessage = "First Name is required")]
            [MaxLength(50)]
            public string Givenname { get; set; } = null!;

            [Required(ErrorMessage = "Last Name is required")]
            [MaxLength(50)]
            public string Surname { get; set; } = null!;

            [Required(ErrorMessage = "Address is required")]
            [MaxLength(50)]
            public string Streetaddress { get; set; } = null!;

            [Required(ErrorMessage = "{0} is required")]
            [MaxLength(50)]
            public string City { get; set; } = null!;


            [Required(ErrorMessage = "{0} is required")]
            [MaxLength(10)]
            public string Zipcode { get; set; } = null!;


            [Required(ErrorMessage = "Country is required")]
            [MaxLength(2)]
            public string CountryCode { get; set; } = null!;



            [Required(ErrorMessage = "Personal Identification Number is required")]
            [MaxLength(20)]
            public string NationalId { get; set; } = null!;


            [Required(ErrorMessage = "{0} is required")]
            public string Telephone { get; set; } = null!;



            [Required(ErrorMessage = "Email is required")]

            [MaxLength(50)]
            public string EmailAddress { get; set; } = null!;


            [Required(ErrorMessage = "{0} is required")]
            [DataType(DataType.Date)]
            public DateTime? Birthday { get; set; }
        }
    }
}
