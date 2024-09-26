interface String
{
    dominitionCapitalize(): string;
    dominitianTrim(): string;
    dominitianIsNullOrEmpty(): boolean;
}
String.prototype.dominitionCapitalize = function (): string
{
    return this.charAt(0).toUpperCase() + this.slice(1);
}

String.prototype.dominitianIsNullOrEmpty = function (): boolean
{
    return this === null || this === '';
}

String.prototype.dominitianTrim = function (): string
{
    if (this === '""') 
    {
        return '';
    }

    return this.trim();
}