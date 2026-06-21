import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { APPLICATION } from '@shared/models/constants';
import { AuthService } from '@shared/services/auth.service';

const TENANT_ID_KEY = `${APPLICATION.name}.TenantId`;
const TENANT_HEADER = `x-${APPLICATION.name.toLowerCase()}-tenantid`;

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const token = inject(AuthService).userSignal()?.access_token;
  const tenantId = sessionStorage.getItem(TENANT_ID_KEY);

  const headers: Record<string, string> = {};
  if (token) headers['Authorization'] = `Bearer ${token}`;
  if (tenantId) headers[TENANT_HEADER] = tenantId;

  if (Object.keys(headers).length === 0) return next(req);
  return next(req.clone({ setHeaders: headers }));
};
