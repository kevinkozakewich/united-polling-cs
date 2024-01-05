import { DatePipe } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatIconModule } from '@angular/material/icon'
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTableModule } from '@angular/material/table';
import { MatToolbarModule } from '@angular/material/toolbar';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'; 

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { loginRequest, msalConfig, protectedResources } from './auth-config';
import { AppHeaderComponent } from './components/app-header/app-header';
import { HomeComponent } from './components/widgets/home/home.component';
import { LoadingComponent } from './components/widgets/loading/loading.component';
import { AppConfig } from '../config/app.config'; 
import { Helper } from './helper'; 
import { PollDialogComponent } from './shared/components/modals/poll-dialog/poll-dialog.component';

import {
    MsalGuard, MsalInterceptor, MsalBroadcastService, MsalInterceptorConfiguration, MsalModule, MsalService,
    MSAL_GUARD_CONFIG, MSAL_INSTANCE, MSAL_INTERCEPTOR_CONFIG, MsalGuardConfiguration, MsalRedirectComponent, ProtectedResourceScopes
} from '@azure/msal-angular';
import { IPublicClientApplication, PublicClientApplication, InteractionType } from '@azure/msal-browser';

export function MSALInstanceFactory(): IPublicClientApplication {
    return new PublicClientApplication(msalConfig);
}

export function MSALInterceptorConfigFactory(): MsalInterceptorConfiguration {
    const protectedResourceMap = new Map<string, Array<string | ProtectedResourceScopes> | null>();

    protectedResourceMap.set(protectedResources.apiPolls.endpoint, [
        {
            httpMethod: 'GET',
            scopes: [...protectedResources.apiPolls.scopes.read]
        },
        {
            httpMethod: 'POST',
            scopes: [...protectedResources.apiPolls.scopes.write]
        },
        {
            httpMethod: 'PUT',
            scopes: [...protectedResources.apiPolls.scopes.write]
        },
        {
            httpMethod: 'DELETE',
            scopes: [...protectedResources.apiPolls.scopes.write]
        }
    ]);

    return {
        interactionType: InteractionType.Popup,
        protectedResourceMap,
    };
}

/**
 * Set your default interaction type for MSALGuard here. If you have any
 * additional scopes you want the user to consent upon login, add them here as well.
 */
export function MSALGuardConfigFactory(): MsalGuardConfiguration {
    return {
        interactionType: InteractionType.Redirect,
        authRequest: loginRequest
    };
}

@NgModule({
    declarations: [
        AppComponent,
        AppHeaderComponent,
        LoadingComponent,
        HomeComponent,
        PollDialogComponent
    ],
    imports: [
        AppRoutingModule,
        BrowserAnimationsModule,
        BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
        FormsModule,
        HttpClientModule,
        MatButtonModule,
        MatCardModule,
        MatCheckboxModule,
        MatDatepickerModule,
        MatDialogModule,
        MatFormFieldModule,
        MatIconModule,
        MatInputModule,
        MatListModule,
        MatNativeDateModule,
        MatProgressSpinnerModule,
        MatTableModule,
        MatToolbarModule,
        MsalModule
    ],
    providers: [
        AppConfig,
        Helper,
        DatePipe,
        Document,
        MsalBroadcastService,
        MsalGuard,
        MsalService,
        {
            provide: HTTP_INTERCEPTORS,
            useClass: MsalInterceptor,
            multi: true
        },
        {
            provide: MSAL_INSTANCE,
            useFactory: MSALInstanceFactory
        },
        {
            provide: MSAL_GUARD_CONFIG,
            useFactory: MSALGuardConfigFactory
        },
        {
            provide: MSAL_INTERCEPTOR_CONFIG,
            useFactory: MSALInterceptorConfigFactory
        }
    ],
    bootstrap: [AppComponent, MsalRedirectComponent]
})
export class AppModule { }
