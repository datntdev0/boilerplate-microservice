import { html } from 'lit';

export interface LayoutErrorProps {
  content: unknown;
}

export const LayoutError = ({ content }: LayoutErrorProps) => html`
<style>
  [data-bs-theme="light"] body {
    background-image: url('/media/images/bg-light.jpg');
  }

  [data-bs-theme="dark"] body {
    background-image: url('/media/images/bg-dark.jpg');
  }
</style>
<div class="d-flex flex-column flex-center flex-column-fluid">
  <div class="d-flex flex-column flex-center text-center p-10">
    <div class="card card-flush w-md-650px py-5">
      <div class="card-body py-15 py-lg-20">
        ${content}
      </div>
    </div>
  </div>
</div>`;