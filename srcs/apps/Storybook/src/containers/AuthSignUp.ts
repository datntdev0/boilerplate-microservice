import { html } from 'lit';

import { LayoutAuth } from '../components/LayoutAuth.js';

export const AuthSignUp = () => html`
${LayoutAuth({
  content: html`
  <div class="d-flex flex-center flex-column flex-column-fluid">
    <form class="form w-100" novalidate>
      <div class="text-center mb-11">
        <h1 class="text-gray-900 fw-bolder mb-3">
          Sign Up
        </h1>
        <div class="text-gray-500 fw-semibold fs-6">
          Your Microservice Architecture
        </div>
      </div>
      <div class="row g-3 mb-9">
        <div class="col-md-6">
          <a href="javascript:void(0);" class="btn btn-flex btn-outline btn-text-gray-700 btn-active-color-primary bg-state-light flex-center text-nowrap w-100">
            <img alt="Logo" src="/media/logos/google-icon.svg" class="h-15px me-3">
            Sign in with Google
          </a>
        </div>
        <div class="col-md-6">
          <a href="javascript:void(0);" class="btn btn-flex btn-outline btn-text-gray-700 btn-active-color-primary bg-state-light flex-center text-nowrap w-100">
            <img alt="Logo" src="/media/logos/apple-light-icon.svg" class="h-15px me-3 theme-light-show">
            <img alt="Logo" src="/media/logos/apple-dark-icon.svg" class="h-15px me-3 theme-dark-show">
            Sign in with Apple
          </a>
        </div>
      </div>
      <div class="separator separator-content my-14">
        <span class="w-125px text-gray-500 fw-semibold fs-7">Or with email</span>
      </div>

      <div class="mt-8">
        <input class="form-control bg-transparent" 
               name="email" 
               placeholder="Email" 
               type="email" 
               required />
        <div class="invalid-feedback"></div>
      </div>

      <div class="row gx-3">
        <div class="col-md-6 mt-4">
          <input class="form-control bg-transparent" 
                 name="firstName" 
                 placeholder="First Name" 
                 type="text" 
                 required />
          <div class="invalid-feedback"></div>
        </div>
        <div class="col-md-6 mt-4">
          <input class="form-control bg-transparent" 
                 name="lastName" 
                 placeholder="Last Name" 
                 type="text" 
                 required />
          <div class="invalid-feedback"></div>
        </div>
      </div>

      <div class="mt-4">
        <input class="form-control bg-transparent" 
               name="password" 
               placeholder="Password" 
               type="password" 
               required />
        <div class="invalid-feedback"></div>
      </div>

      <div class="mt-4">
        <input class="form-control bg-transparent" 
               name="confirmPassword" 
               placeholder="Confirm Password" 
               type="password" 
               required />
        <div class="invalid-feedback"></div>
      </div>

      <div class="my-8">
        <label class="form-check form-check-inline">
          <input class="form-check-input" type="checkbox" name="toc" value="1" required>
          <span class="form-check-label fw-semibold text-gray-700 fs-base ms-1">
            I Accept the <a href="#" class="link-primary">Terms</a>
          </span>
        </label>
        <div class="invalid-feedback"></div>
      </div>

      <div class="d-grid mb-10">
        <button type="submit" class="btn btn-primary">
          <span class="indicator-label">Sign Up</span>
        </button>
      </div>

      <div class="text-gray-500 text-center fw-semibold fs-6">
        Already have an Account? <a href="/auth/signin" class="link-primary">Sign in</a>
      </div>
    </form>
  </div>`
})}
`;