import { Provider } from "@angular/core";
import { environment } from "@envs/environment";
import { API_BASE_URL_ADMIN, SrvAdminClientProxy } from "@shared/proxies/srv-admin-proxies";
import { API_BASE_URL_IDENTITY, SrvIdentityClientProxy } from "@shared/proxies/srv-identity-proxies";

export function provideSrvIdentityProxy(): Provider[] {
  return [
    SrvIdentityClientProxy, 
    { provide: API_BASE_URL_IDENTITY, useValue: `${environment.apiUrl}/srv-identity` }
  ];
}

export function provideSrvAdminProxy(): Provider[] {
  return [
    SrvAdminClientProxy,
    { provide: API_BASE_URL_ADMIN, useValue: `${environment.apiUrl}/srv-admin` }
  ];
}