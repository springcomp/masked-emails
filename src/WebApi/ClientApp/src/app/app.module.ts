import { AppRoutingModule } from './app-routing.module';
import { APP_INITIALIZER, NgModule } from '@angular/core';
import { AuthModule, ConfigResult, OidcConfigService, OidcSecurityService, OpenIdConfiguration } from 'angular-auth-oidc-client';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CoreModule } from './core';
import { HttpClientModule } from '@angular/common/http';
import { GravatarModule } from 'ngx-gravatar';
import { BrowserAnimationsModule, NoopAnimationsModule } from '@angular/platform-browser/animations';
import { AuthorizationGuard } from './core/authorization-guard';
import { environment } from '../environments/environment';
import { FontAwesomeModule, FaIconLibrary } from '@fortawesome/angular-fontawesome';
import { fas } from '@fortawesome/free-solid-svg-icons';

//Import material module
import { MaterialModule } from './material.module';

//Components
import { AddressesComponent } from './addresses/addresses.component';
import { AppComponent } from './app.component';
import { AppContainerComponent } from './app-container/app-container.component';
import { AuthCallbackComponent } from './auth-callback/auth-callback.component';
import { EditForwardingAddressComponent } from './app-container/edit-forwarding-address/edit-forwarding-address.component'
import { HomeComponent } from './home/home.component';
import { InboxComponent } from './inbox/inbox.component';
import { LoginComponent } from './login/login.component'
import { MaskedEmailsComponent } from './masked-emails/masked-emails.component';
import { MessagesComponent } from './messages/messages.component';
import { NewMaskedEmailAddressDialogComponent } from './addresses/new-masked-email-address-dialog/new-masked-email-address-dialog.component';
import { ProfileDialogComponent } from './app-container/profile-dialog/profile-dialog.component';
import { UpdateMaskedEmailAddressDialogComponent } from './addresses/update-masked-email-address-dialog/update-masked-email-address-dialog.component';
import { UserButtonComponent } from './app-container/user-button/user-button.component';

import { LoaderService } from './shared/services/loader.service';

export function loadConfig(oidcConfigService: OidcConfigService) {
  return () => {
    const oidc_configuration = 'assets/auth.clientConfiguration.json';
    const oidc_configuration_prod = 'assets/auth.clientConfiguration.prod.json';

    var configuration = oidc_configuration;
    if (environment.production)
      configuration = oidc_configuration_prod;

    console.log(`loading ${configuration} OpenId configuration.`);

    oidcConfigService.load(configuration);
  }
}

@NgModule({
  declarations: [
    AddressesComponent,
    AppComponent,
    AppContainerComponent,
    AuthCallbackComponent,
    EditForwardingAddressComponent,
    HomeComponent,
    InboxComponent,
    LoginComponent,
    MaskedEmailsComponent,
    MessagesComponent,
    NewMaskedEmailAddressDialogComponent,
    ProfileDialogComponent,
    UpdateMaskedEmailAddressDialogComponent,
    UserButtonComponent
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
      useFactory: loadConfig,
      deps: [OidcConfigService],
      multi: true,
    },
    LoaderService
  ],
  bootstrap: [AppComponent],
})
export class AppModule {
  constructor(private openId: OidcSecurityService, private configService: OidcConfigService, library: FaIconLibrary) {
    library.addIconPacks(fas);

    this.configService.onConfigurationLoaded.subscribe((configResult: ConfigResult) => {

      this.openId.setupModule(
        configResult.customConfig,
        configResult.authWellknownEndpoints
      );
    });
  }
}
