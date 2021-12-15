import { AuthModule, OpenIdConfiguration, StsConfigHttpLoader, StsConfigLoader } from 'angular-auth-oidc-client';
import { environment } from '../environments/environment';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { NgModule } from '@angular/core';

export const httpLoaderFactory = (httpClient: HttpClient) => {

  const oidc_configuration = 'assets/auth.clientConfiguration.json';
  const oidc_configuration_prod = 'assets/auth.clientConfiguration.prod.json';

  var configuration = oidc_configuration;
  if (environment.production)
    configuration = oidc_configuration_prod;

  const requestUri: string = `${window.location.origin}/${configuration}`;

  console.log(`loading ${configuration} OpenId configuration.`);
  console.log(`GET ${requestUri} HTTP/1.1`);

  const config$ = httpClient.get<any>(requestUri).pipe(
    map((customConfig: any) => {
      const config: OpenIdConfiguration = {
        authority: customConfig.stsServer,
        redirectUrl: window.location.origin + "/",
        clientId: customConfig.client_id,
        responseType: customConfig.response_type,
        scope: customConfig.scope,
        postLogoutRedirectUri: customConfig.post_logout_redirect_uri,
        startCheckSession: customConfig.start_checksession,
        silentRenew: true,
        silentRenewUrl: customConfig.silent_renew_url,
        postLoginRoute: customConfig.startup_route,
        forbiddenRoute: customConfig.forbidden_route,
        unauthorizedRoute: customConfig.unauthorized_route,
        logLevel: customConfig.log_level, 
        maxIdTokenIatOffsetAllowedInSeconds: customConfig.max_id_token_iat_offset_allowed_in_seconds,
        historyCleanupOff: true
      };
      return config;
    })
  );

  return new StsConfigHttpLoader(config$);
};

@NgModule({
  imports: [
    AuthModule.forRoot({
      loader: {
        provide: StsConfigLoader,
        useFactory: httpLoaderFactory,
        deps: [HttpClient],
      },
    }),
  ],
  exports: [AuthModule],
})
export class AuthConfigModule {}