export abstract class ValidatorConstants
{
    public static mailPlaceholder: string = 'account@website.com';
    public static passwordControlName: string = 'password';
    public static emailControlName: string = 'email';
    public static confirmPasswordControlName: string = 'confirmPassword';
    public static rememberMeControlName: string = 'rememberMe'
    public static passwrodFormGroupControlName: string = 'passwordFormGroup';
    public static passwordRegex: RegExp = new RegExp(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/);
}