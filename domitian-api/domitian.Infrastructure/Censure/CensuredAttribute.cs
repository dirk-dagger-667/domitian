namespace domitian.Infrastructure.Censure
{
  //Flagging attribute for props that need to be masked/censured, ex. => passwords, etc.
  [AttributeUsage(AttributeTargets.Property)]
  public class CensuredAttribute : Attribute { }
}
