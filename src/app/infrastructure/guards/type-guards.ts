// Fucntion that checks whether a value has been defined(instantiated)
export function isDefined<T>(value: T | null | undefined): value is T
{
    return !(value == null);
}

// Function that checks if an unknown value is a primitive
export function isPrimitive(value: unknown):
    value is string | number | boolean | symbol | bigint | undefined | null
{
    return (
        typeof value === "string" ||
        typeof value === "number" ||
        typeof value === "boolean" ||
        typeof value === "symbol" ||
        typeof value === "bigint" ||
        value === "undefined" ||
        value === "null"
    )
}

// Function to check if an object is of a specific complex type
export function isOfComplexType<T extends object>(value: T): value is T
{
    let instOfE = createInstance<T>(value);
    return typeof value !== typeof instOfE;
}

// Function to check if an unknown is a complex type
export function isComplexType(value: unknown): value is object
{
    return typeof value === "object" && value !== null && !Array.isArray(value);
}

// Function to check if an array is an array of complex types
export function isArrayOfComplexTypes(arr: unknown[]): arr is object[]
{
    return arr.findIndex(e => !isComplexType(e)) > -1;
}

// Function to check if an array is of only a specific complex type
export function isArrayOfTheSameComplexType<T extends object>(arr: T[]): arr is T[]
{
    if(!isArrayOfComplexTypes(arr))
        return false;

    return arr.findIndex((e) => isOfComplexType<T>(e)) > -1;
}

// Function that creates an instance of an object 
function createInstance<T extends object>(proto: T): T
{
    return Object.create(proto);
}
