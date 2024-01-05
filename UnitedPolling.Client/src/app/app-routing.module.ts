import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { MsalGuard, MsalRedirectComponent } from '@azure/msal-angular';
import { BrowserUtils } from '@azure/msal-browser';

import { HomeComponent } from './components/widgets/home/home.component';
import { LoadingComponent } from './components/widgets/loading/loading.component';

const routes: Routes = [
    { path: '', component: LoadingComponent, pathMatch: 'full' },
    { path: 'home', component: HomeComponent, canActivate: [MsalGuard] },
    { path: 'loading', component: LoadingComponent },
    {
        // Needed for handling redirect after login
        path: 'auth',
        component: MsalRedirectComponent
    },
];

@NgModule({
    imports: [RouterModule.forRoot(routes, {
        // Don't perform initial navigation in iframes or popups
        initialNavigation: !BrowserUtils.isInIframe() && !BrowserUtils.isInPopup() ? 'enabledNonBlocking' : 'disabled' // Set to enabledBlocking to use Angular Universal
    })],
    exports: [RouterModule]
})
export class AppRoutingModule { }
