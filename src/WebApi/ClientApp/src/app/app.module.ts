import { AppRoutingModule } from './app-routing.module';
import { APP_INITIALIZER, NgModule } from '@angular/core';
import { AuthModule, OidcConfigService, OidcSecurityService, OpenIdConfiguration } from 'angular-auth-oidc-client';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CoreModule } from './core';
import { GravatarModule } from 'ngx-gravatar';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AuthorizationGuard } from './core/authorization-guard';
import { environment } from '../environments/environment';
import { FontAwesomeModule, FaIconLibrary } from '@fortawesome/angular-fontawesome';
import { fas } from '@fortawesome/free-solid-svg-icons';
import { map, switchMap } from 'rxjs/operators';
import { HttpClient, HttpClientModule } from '@angular/common/http';

//Import material module
import { MaterialModule } from './material.module';

//Components
import { AddressesComponent } from './addresses/addresses.component';
import { AppComponent } from './app.component';
import { AppContainerComponent } from './app-container/app-container.component';
import { EditForwardingAddressComponent } from './app-container/edit-forwarding-address/edit-forwarding-address.component'
import { HomeComponent } from './home/home.component';
import { InboxComponent } from './inbox/inbox.component';
import { LoginComponent } from './login/login.component'
import { MaskedEmailsComponent } from './masked-emails/masked-emails.component';
import { MessagesComponent } from './messages/messages.component';
import { MessageDialogComponent } from './messages/message-dialog/message-dialog.component';
import { NewMaskedEmailAddressDialogComponent } from './addresses/new-masked-email-address-dialog/new-masked-email-address-dialog.component';
import { ProfileDialogComponent } from './app-container/profile-dialog/profile-dialog.component';
import { UpdateMaskedEmailAddressDialogComponent } from './addresses/update-masked-email-address-dialog/update-masked-email-address-dialog.component';
import { UserButtonComponent } from './app-container/user-button/user-button.component';

import { MessageContentMobileViewComponent } from './messages/message-content-mobile-view/message-content-mobile-view.component';
import { MessagesTableMobileViewComponent } from './messages/messages-table-mobile-view/messages-table-mobile-view.component';
import { MessagesTableViewComponent } from './messages/messages-table-view/messages-table-view.component';
import { MessageContentViewComponent } from './messages/message-content-view/message-content-view.component';
import { AddressesTableViewComponent } from './addresses/addresses-table-view/addresses-table-view.component';
import { AddressesTableMobileViewComponent } from './addresses/addresses-table-mobile-view/addresses-table-mobile-view.component';
import { LoadingScreenComponent } from './loading-screen/loading-screen.component';


export function configureAuth(oidcConfigService: OidcConfigService, httpClient: HttpClient) {
  const oidc_configuration = 'assets/auth.clientConfiguration.json';
  const oidc_configuration_prod = 'assets/auth.clientConfiguration.prod.json';

  var configuration = oidc_configuration;
  if (environment.production)
    configuration = oidc_configuration_prod;

  console.log(`loading ${configuration} OpenId configuration.`);
  const setupAction$ = httpClient.get<any>(`${window.location.origin}/${configuration}`).pipe(
    map((customConfig) => {
      return {
        stsServer: customConfig.stsServer,
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
    }),
    switchMap((config) => oidcConfigService.withConfig(config))
  );

  return () => setupAction$.toPromise();
}


@NgModule({
  declarations: [
    AddressesComponent,
    AppComponent,
    AppContainerComponent,
    EditForwardingAddressComponent,
    HomeComponent,
    InboxComponent,
    LoginComponent,
    MaskedEmailsComponent,
    MessageDialogComponent,
    MessagesComponent,
    NewMaskedEmailAddressDialogComponent,
    ProfileDialogComponent,
    UpdateMaskedEmailAddressDialogComponent,
    UserButtonComponent,
    MessageContentMobileViewComponent,
    MessagesTableMobileViewComponent,
    MessagesTableViewComponent,
    MessageContentViewComponent,
    AddressesTableViewComponent,
    AddressesTableMobileViewComponent,
    LoadingScreenComponent
  ],
  entryComponents: [
    ProfileDialogComponent,
    UpdateMaskedEmailAddressDialogComponent,
    NewMaskedEmailAddressDialogComponent
  ],
  imports: [
    AppRoutingModule,
    AuthModule.forRoot(),
    BrowserAnimationsModule,
    BrowserModule,
    CoreModule,
    FontAwesomeModule,
    FormsModule,
    GravatarModule,
    HttpClientModule,
    MaterialModule,
    ReactiveFormsModule
  ],
  providers: [
    AuthorizationGuard,
    OidcConfigService,
    {
      provide: APP_INITIALIZER,
      useFactory: configureAuth,
      deps: [OidcConfigService, HttpClient],
      multi: true,
    }
  ],
  bootstrap: [AppComponent],
})
export class AppModule {
  constructor(library: FaIconLibrary) {
    library.addIconPacks(fas);
  }
}
