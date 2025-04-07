import { z } from "zod";

 export const loginResponseSchema = z.object({
    email: z.string().email(),
    bearerToken: z.string(),
    refreshToken: z.string(),
});

export interface LoginResponse 
{
    email: string | null;
    bearerToken: string | null;
    refreshToken: string | null;
}