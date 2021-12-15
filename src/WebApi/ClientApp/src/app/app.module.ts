import { AppRoutingModule } from './app-routing.module';
import { AuthConfigModule } from './auth-config.module';
import { AuthorizationGuard } from './core/authorization-guard';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BrowserModule } from '@angular/platform-browser';
import { CoreModule } from './core';
import { fas } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeModule, FaIconLibrary } from '@fortawesome/angular-fontawesome';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { GravatarModule } from 'ngx-gravatar';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';

//Import material module
import { MaterialModule } from './material.module';

//Components
import { AddressesComponent } from './addresses/addresses.component';
import { AppComponent } from './app.component';
import { AppContainerComponent } from './app-container/app-container.component';
import { EditForwardingAddressComponent } from './app-container/edit-forwarding-address/edit-forwarding-address.component'
import { HomeComponent } from './home/home.component';
import { InboxComponent } from './inbox/inbox.component';
import { LoadingScreenComponent } from './loading-screen/loading-screen.component';
import { LoginComponent } from './login/login.component'
import { MaskedEmailsComponent } from './masked-emails/masked-emails.component';
import { MessageDialogComponent } from './messages/message-dialog/message-dialog.component';
import { MessagesComponent } from './messages/messages.component';
import { NewMaskedEmailAddressDialogComponent } from './addresses/new-masked-email-address-dialog/new-masked-email-address-dialog.component';
import { ProfileDialogComponent } from './app-container/profile-dialog/profile-dialog.component';
import { RemoveMaskedEmailAddressDialogComponent } from './addresses/remove-masked-email-address-dialog/remove-masked-email-address-dialog.component';
import { UpdateMaskedEmailAddressDialogComponent } from './addresses/update-masked-email-address-dialog/update-masked-email-address-dialog.component';
import { UserButtonComponent } from './app-container/user-button/user-button.component';

import { AddressesTableMobileViewComponent } from './addresses/addresses-table-mobile-view/addresses-table-mobile-view.component';
import { AddressesTableViewComponent } from './addresses/addresses-table-view/addresses-table-view.component';
import { MessageContentMobileViewComponent } from './messages/message-content-mobile-view/message-content-mobile-view.component';
import { MessageContentViewComponent } from './messages/message-content-view/message-content-view.component';
import { MessagesTableMobileViewComponent } from './messages/messages-table-mobile-view/messages-table-mobile-view.component';
import { MessagesTableViewComponent } from './messages/messages-table-view/messages-table-view.component';

@NgModule({
  declarations: [
    AddressesComponent,
    AppComponent,
    AppContainerComponent,
    EditForwardingAddressComponent,
    HomeComponent,
    InboxComponent,
    LoadingScreenComponent,
    LoginComponent,
    MaskedEmailsComponent,
    MessagesComponent,
    UserButtonComponent,

    MessageDialogComponent,
    NewMaskedEmailAddressDialogComponent,
    ProfileDialogComponent,
    RemoveMaskedEmailAddressDialogComponent,
    UpdateMaskedEmailAddressDialogComponent,

    AddressesTableMobileViewComponent,
    AddressesTableViewComponent,
    MessageContentMobileViewComponent,
    MessageContentViewComponent,
    MessagesTableMobileViewComponent,
    MessagesTableViewComponent,
  ],
  entryComponents: [
    NewMaskedEmailAddressDialogComponent,
    ProfileDialogComponent,
    UpdateMaskedEmailAddressDialogComponent,
  ],
  imports: [
    AppRoutingModule,
    AuthConfigModule,
    BrowserAnimationsModule,
    BrowserModule,
    CoreModule,
    FontAwesomeModule,
    FormsModule,
    GravatarModule,
    HttpClientModule,
    MaterialModule,
    ReactiveFormsModule,
  ],
  providers: [
    AuthorizationGuard,
  ],
  bootstrap: [AppComponent],
})
export class AppModule {
  constructor(library: FaIconLibrary) {
    library.addIconPacks(fas);
  }
}
