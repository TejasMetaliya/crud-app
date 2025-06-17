export interface AuthResponse {
  isSuccess: boolean;
  token: string;
  expiration: string;
  errorMessage: string | null;
}