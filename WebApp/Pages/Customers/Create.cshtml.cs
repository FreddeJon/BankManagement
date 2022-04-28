using Application.Features.Customer.Commands;
#pragma warning disable CS8618

namespace WebApp.Pages.Customers;

[Authorize(Roles = nameof(ApplicationRoles.Admin))]
public class CreateModel : PageModel
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public CreateModel(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    [BindProperty] public CreateCustomerModel Customer { get; set; }




    public List<SelectListItem> Countries { get; set; }



    public void OnGet()
    {
        SetUpDropdowns();
    }





    public async Task<IActionResult> OnPostCreate()
    {
        SetUpDropdowns();

        if (!ModelState.IsValid) return Page();

        var customer = _mapper.Map<CustomerDto>(Customer);

        var response = await _mediator.Send(new CreateCustomerCommand() { Customer = customer });

        if (response.Status == Application.Responses.StatusCode.Error)
        {
            response.Errors?.ForEach(x => ModelState.AddModelError(x.PropertyName, x.ErrorMessage));
            return Page();
        }


        TempData["Message"] = "Customer Created";
        return Redirect($"/Customers/{response.CustomerId}");
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
    public class CreateCustomerModel
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