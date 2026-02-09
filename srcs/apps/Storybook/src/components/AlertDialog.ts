import { html } from 'lit';

export type AlertType = 'success' | 'danger' | 'warning' | 'info';

export interface AlertDialogProps {
  /** Alert type - determines icon and color scheme */
  type: AlertType;
  /** Dialog title */
  title?: string;
  /** Dialog message/text content */
  text?: string;
  /** Confirm button text */
  confirmButtonText?: string;
  /** Confirm button variant */
  confirmButtonVariant?: 'primary' | 'secondary' | 'success' | 'danger' | 'warning' | 'info';
  /** Cancel button text */
  cancelButtonText?: string;
  /** Cancel button variant */
  cancelButtonVariant?: 'primary' | 'secondary' | 'success' | 'danger' | 'warning' | 'info';
}


const getIconForType = (type: AlertType) => {
  const icons = {
    success: html`
      <div class="alert-icon alert-icon-success">
        <svg xmlns="http://www.w3.org/2000/svg" width="80" height="80" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
          <circle cx="12" cy="12" r="10"></circle>
          <path d="M8 12l3 3 5-5"></path>
        </svg>
      </div>
    `,
    danger: html`
      <div class="alert-icon alert-icon-error">
        <svg xmlns="http://www.w3.org/2000/svg" width="80" height="80" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
          <circle cx="12" cy="12" r="10"></circle>
          <line x1="15" y1="9" x2="9" y2="15"></line>
          <line x1="9" y1="9" x2="15" y2="15"></line>
        </svg>
      </div>
    `,
    warning: html`
      <div class="alert-icon alert-icon-warning">
        <svg xmlns="http://www.w3.org/2000/svg" width="80" height="80" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
          <path d="M10.29 3.86L1.82 18a2 2 0 0 0 1.71 3h16.94a2 2 0 0 0 1.71-3L13.71 3.86a2 2 0 0 0-3.42 0z"></path>
          <line x1="12" y1="9" x2="12" y2="13"></line>
          <line x1="12" y1="17" x2="12.01" y2="17"></line>
        </svg>
      </div>
    `,
    info: html`
      <div class="alert-icon alert-icon-info">
        <svg xmlns="http://www.w3.org/2000/svg" width="80" height="80" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
          <circle cx="12" cy="12" r="10"></circle>
          <line x1="12" y1="16" x2="12" y2="12"></line>
          <line x1="12" y1="8" x2="12.01" y2="8"></line>
        </svg>
      </div>
    `,
  };
  return icons[type];
};

export const AlertDialog = ({
  type = 'info',
  title,
  text,
  confirmButtonText = 'Confirm',
  confirmButtonVariant = 'primary',
  cancelButtonText = 'Cancel',
  cancelButtonVariant = 'secondary',
}: AlertDialogProps) => {
  const overlayClasses = `alert-dialog-overlay d-flex align-items-center justify-content-center`;

  return html`
    <div class="${overlayClasses}" @click=${(e: Event) => e.stopPropagation()}>
      <div class="alert-dialog">
        <div class="modal-content">
          <div class="modal-body text-center p-4">
            ${getIconForType(type)}
            
            ${title ? html`<h2 class="alert-title">${title}</h2>` : ''}
            ${text ? html`<p class="alert-text">${text}</p>` : ''}
          </div>

          <div class="modal-footer border-0 justify-content-center pb-4">
            <div class="alert-actions">
              ${cancelButtonText ? html`
                <button type="button" class="btn btn-${cancelButtonVariant ?? 'secondary'}">
                  ${cancelButtonText}
                </button>
              ` : ''}
              ${confirmButtonText ? html`
                <button type="button" class="btn btn-${confirmButtonVariant}">
                  ${confirmButtonText}
                </button>
              ` : ''}
            </div>
          </div>
        </div>
      </div>
    </div>
  `;
};
