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
            {
               
            }


            return Page();
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
            [Required(ErrorMessage = "First Name is required")]
            [MaxLength(50)]
            public string Givenname { get; set; }

            [Required(ErrorMessage = "Last Name is required")]
            [MaxLength(50)]
            public string Surname { get; set; }

            [Required(ErrorMessage = "Address is required")]
            [MaxLength(50)]
            public string Streetaddress { get; set; }

            [Required(ErrorMessage = "{0} is required")]
            [MaxLength(50)]
            public string City { get; set; }


            [Required(ErrorMessage = "{0} is required")]
            [MaxLength(10)]
            public string Zipcode { get; set; }


            [Required(ErrorMessage = "Country is required")]
            [MaxLength(2)]
            public string CountryCode { get; set; }



            [Required(ErrorMessage = "Personal Identification Number is required")]
            [MaxLength(20)]
            public string NationalId { get; set; }


            [Required(ErrorMessage = "{0} is required")]
            public string Telephone { get; set; }



            [Required(ErrorMessage = "Email is required")]

            [MaxLength(50)]
            public string EmailAddress { get; set; }


            [Required(ErrorMessage = "{0} is required")]
            [DataType(DataType.Date)]
            public DateTime? Birthday { get; set; }
        }
    }
}
