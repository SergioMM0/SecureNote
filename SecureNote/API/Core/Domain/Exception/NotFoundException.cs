namespace API.Core.Domain.Exception;

public class NotFoundException : System.Exception {
    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundException"/> class.
    /// </summary>
    public NotFoundException() : base("Not found") {
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundException"/> class.
    /// </summary>
    /// <param name="error">The error message associated with the exception.</param>
    public NotFoundException(string error) : base(error) {
    }
}