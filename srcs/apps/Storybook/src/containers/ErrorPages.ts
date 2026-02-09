import { html } from "lit";

import { LayoutError } from "../components/LayoutError";

export interface ErrorPagesProps {
  title: string;
  description: string;
  imageUrl: string;
  action: string;
}

export const ErrorPages = ({ title, description, imageUrl, action: action }: ErrorPagesProps) => html`
${LayoutError({
  content: html`
  <h1 class="fw-bolder text-gray-900 mb-5">${title}</h1>
  <div class="fw-semibold fs-6 text-gray-500 mb-7">
    ${description}
  </div>
  <img src="${imageUrl}" class="my-16 mw-100 mh-300px" alt="Error Illustration">
  <div>
    <a href="javascript:void(0);" class="btn btn-sm btn-primary">${action}</a>
  </div>`
})}
`;
