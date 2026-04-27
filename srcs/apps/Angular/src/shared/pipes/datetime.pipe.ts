import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'dateTime',
  standalone: false
})
export class DateTimePipe implements PipeTransform {
  private readonly months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];

  /**
   * Main transform method - defaults to standard format with seconds
   * @param value - Date to format
   * @param includeSeconds - Whether to include seconds in output (default: true)
   */
  transform(value: string | Date | null | undefined, includeSeconds: boolean = true): string {
    if (!value) return '';
    
    const date = this.parseDate(value);

    const localYear = date.getFullYear();
    const localMonth = String(date.getMonth() + 1).padStart(2, '0');
    const localDay = String(date.getDate()).padStart(2, '0');
    const localHours = String(date.getHours()).padStart(2, '0');
    const localMinutes = String(date.getMinutes()).padStart(2, '0');
    const localSeconds = String(date.getSeconds()).padStart(2, '0');

    if (includeSeconds) {
      return `${localYear}-${localMonth}-${localDay} ${localHours}:${localMinutes}:${localSeconds}`;
    } else {
      return `${localYear}-${localMonth}-${localDay} ${localHours}:${localMinutes}`;
    }
  }

  /**
   * Format date as relative time (e.g., "5 minutes ago", "2 days ago")
   * For dates >= 7 days old, returns readable format
   */
  relative(value: string | Date | null | undefined): string {
    if (!value) return '';

    const date = this.parseDate(value);
    const now = new Date();
    const diffMs = now.getTime() - date.getTime();
    const diffSeconds = Math.floor(diffMs / 1000);
    const diffMinutes = Math.floor(diffSeconds / 60);
    const diffHours = Math.floor(diffMinutes / 60);
    const diffDays = Math.floor(diffHours / 24);

    // If >= 7 days or future date, return readable format
    if (diffDays >= 7 || diffSeconds < 0) {
      return this.readable(date);
    }

    // < 60 seconds
    if (diffSeconds < 60) {
      return diffSeconds <= 5 ? 'just now' : `${diffSeconds} ${diffSeconds === 1 ? 'second' : 'seconds'} ago`;
    }

    // < 60 minutes
    if (diffMinutes < 60) {
      return `${diffMinutes} ${diffMinutes === 1 ? 'minute' : 'minutes'} ago`;
    }

    // < 24 hours
    if (diffHours < 24) {
      return `${diffHours} ${diffHours === 1 ? 'hour' : 'hours'} ago`;
    }

    // < 7 days
    return `${diffDays} ${diffDays === 1 ? 'day' : 'days'} ago`;
  }

  /**
   * Format date in readable format: 'DD MMM YYYY, HH:mm'
   * Example: "27 Apr 2026, 14:30"
   */
  readable(value: string | Date | null | undefined): string {
    if (!value) return '';
    
    const date = this.parseDate(value);
    
    const day = String(date.getDate()).padStart(2, '0');
    const month = this.months[date.getMonth()];
    const year = date.getFullYear();
    const hours = String(date.getHours()).padStart(2, '0');
    const minutes = String(date.getMinutes()).padStart(2, '0');

    return `${day} ${month} ${year}, ${hours}:${minutes}`;
  }

  /**
   * Parse date string or Date object, handling UTC conversion
   */
  private parseDate(value: string | Date): Date {
    if (value instanceof Date) {
      return value;
    }
    // Add 'Z' suffix if not present to ensure UTC parsing
    return new Date(value.endsWith('Z') ? value : `${value}Z`);
  }

  /**
   * Static helper method to format date without seconds
   * Kept for backward compatibility
   */
  static formatWithoutSeconds(value: string | Date | null | undefined): string {
    if (!value) return '';
    
    const date = value instanceof Date ? value : new Date(value.endsWith('Z') ? value : `${value}Z`);
    
    const localYear = date.getFullYear();
    const localMonth = String(date.getMonth() + 1).padStart(2, '0');
    const localDay = String(date.getDate()).padStart(2, '0');
    const localHours = String(date.getHours()).padStart(2, '0');
    const localMinutes = String(date.getMinutes()).padStart(2, '0');

    return `${localYear}-${localMonth}-${localDay} ${localHours}:${localMinutes}`;
  }
}
