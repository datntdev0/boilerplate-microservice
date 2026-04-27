import { describe, it, expect, beforeEach, vi } from 'vitest';
import { DateTimePipe } from './datetime.pipe';

describe('Pipes.DateTimePipe', () => {
  let pipe: DateTimePipe;

  beforeEach(() => {
    pipe = new DateTimePipe();
  });

  describe('transform (default format)', () => {
    it('should create an instance', () => {
      expect(pipe).toBeTruthy();
    });

    it('should return empty string for null value', () => {
      expect(pipe.transform(null)).toBe('');
    });

    it('should return empty string for undefined value', () => {
      expect(pipe.transform(undefined)).toBe('');
    });

    it('should transform ISO string to local datetime string with seconds', () => {
      const date = new Date('2024-04-26T12:30:45.000Z');
      const result = pipe.transform(date);
      
      expect(result).toMatch(/^\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}$/);
    });

    it('should transform without seconds when includeSeconds is false', () => {
      const date = new Date('2024-04-26T12:30:45.000Z');
      const result = pipe.transform(date, false);
      
      expect(result).toMatch(/^\d{4}-\d{2}-\d{2} \d{2}:\d{2}$/);
      expect(result).not.toMatch(/:\d{2}$/); // Should not end with seconds
    });

    it('should handle Date object input', () => {
      const date = new Date('2024-12-25T09:15:30.000Z');
      const result = pipe.transform(date);
      
      expect(result).toMatch(/^\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}$/);
    });

    it('should handle ISO string without Z suffix', () => {
      const isoString = '2024-06-15T14:45:30';
      const result = pipe.transform(isoString);
      
      expect(result).toMatch(/^\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}$/);
    });
  });

  describe('relative', () => {
    beforeEach(() => {
      // Mock current date to 2026-04-27 12:00:00
      vi.useFakeTimers();
      vi.setSystemTime(new Date('2026-04-27T12:00:00Z'));
    });

    afterEach(() => {
      vi.useRealTimers();
    });

    it('should return "just now" for very recent dates', () => {
      const date = new Date('2026-04-27T11:59:58Z'); // 2 seconds ago
      const result = pipe.relative(date);
      
      expect(result).toBe('just now');
    });

    it('should return seconds ago', () => {
      const date = new Date('2026-04-27T11:59:30Z'); // 30 seconds ago
      const result = pipe.relative(date);
      
      expect(result).toBe('30 seconds ago');
    });

    it('should use singular for 1 second', () => {
      const date = new Date('2026-04-27T11:59:59Z'); // 1 second ago
      const result = pipe.relative(date);
      
      expect(result).toBe('1 second ago');
    });

    it('should return minutes ago', () => {
      const date = new Date('2026-04-27T11:45:00Z'); // 15 minutes ago
      const result = pipe.relative(date);
      
      expect(result).toBe('15 minutes ago');
    });

    it('should use singular for 1 minute', () => {
      const date = new Date('2026-04-27T11:59:00Z'); // 1 minute ago
      const result = pipe.relative(date);
      
      expect(result).toBe('1 minute ago');
    });

    it('should return hours ago', () => {
      const date = new Date('2026-04-27T08:00:00Z'); // 4 hours ago
      const result = pipe.relative(date);
      
      expect(result).toBe('4 hours ago');
    });

    it('should use singular for 1 hour', () => {
      const date = new Date('2026-04-27T11:00:00Z'); // 1 hour ago
      const result = pipe.relative(date);
      
      expect(result).toBe('1 hour ago');
    });

    it('should return days ago', () => {
      const date = new Date('2026-04-24T12:00:00Z'); // 3 days ago
      const result = pipe.relative(date);
      
      expect(result).toBe('3 days ago');
    });

    it('should use singular for 1 day', () => {
      const date = new Date('2026-04-26T12:00:00Z'); // 1 day ago
      const result = pipe.relative(date);
      
      expect(result).toBe('1 day ago');
    });

    it('should return readable format for dates >= 7 days', () => {
      const date = new Date('2026-04-15T12:00:00Z'); // 12 days ago
      const result = pipe.relative(date);
      
      expect(result).toMatch(/^\d{2} \w{3} \d{4}, \d{2}:\d{2}$/);
      expect(result).toContain('Apr 2026');
    });

    it('should return readable format for future dates', () => {
      const date = new Date('2026-04-28T12:00:00Z'); // Tomorrow
      const result = pipe.relative(date);
      
      expect(result).toMatch(/^\d{2} \w{3} \d{4}, \d{2}:\d{2}$/);
    });
  });

  describe('readable', () => {
    it('should return empty string for null value', () => {
      expect(pipe.readable(null)).toBe('');
    });

    it('should format date as "DD MMM YYYY, HH:mm"', () => {
      const date = new Date('2026-04-27T14:30:00Z');
      const result = pipe.readable(date);
      
      expect(result).toMatch(/^\d{2} \w{3} \d{4}, \d{2}:\d{2}$/);
      expect(result).toContain('Apr 2026');
    });

    it('should handle different months', () => {
      const date = new Date('2026-12-25T09:15:00Z');
      const result = pipe.readable(date);
      
      expect(result).toContain('Dec 2026');
    });

    it('should pad day with zero', () => {
      const date = new Date('2026-01-05T10:00:00Z');
      const result = pipe.readable(date);
      
      expect(result).toMatch(/^05 Jan/);
    });

    it('should handle ISO string input', () => {
      const isoString = '2026-04-27T14:30:00Z';
      const result = pipe.readable(isoString);
      
      expect(result).toMatch(/^\d{2} \w{3} \d{4}, \d{2}:\d{2}$/);
    });
  });

  describe('formatWithoutSeconds (static method)', () => {
    it('should format date without seconds', () => {
      const date = new Date('2024-04-26T12:30:45.000Z');
      const result = DateTimePipe.formatWithoutSeconds(date);
      
      expect(result).toMatch(/^\d{4}-\d{2}-\d{2} \d{2}:\d{2}$/);
      expect(result).not.toMatch(/:\d{2}:\d{2}$/); // Should not have seconds
    });

    it('should return empty string for null', () => {
      expect(DateTimePipe.formatWithoutSeconds(null)).toBe('');
    });
  });
});
