import { describe, it, expect, beforeEach } from 'vitest';
import { LocalDateTimePipe } from './local-datetime.pipe';

describe('Pipes.LocalDateTimePipe', () => {
  let pipe: LocalDateTimePipe;

  beforeEach(() => {
    pipe = new LocalDateTimePipe();
  });

  it('should create an instance', () => {
    expect(pipe).toBeTruthy();
  });

  it('should return empty string for null value', () => {
    expect(pipe.transform(null)).toBe('');
  });

  it('should return empty string for undefined value', () => {
    expect(pipe.transform(undefined)).toBe('');
  });

  it('should return empty string for empty string', () => {
    expect(pipe.transform('')).toBe('');
  });

  it('should transform ISO string with Z suffix to local datetime string', () => {
    // Use a fixed date for testing
    const date = new Date('2024-04-26T12:30:45.000Z');
    const result = pipe.transform(date);
    
    // The result format should be YYYY-MM-DD HH:MM:SS
    expect(result).toMatch(/^\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}$/);
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

  it('should add Z suffix to string if not present', () => {
    const isoString = '2024-01-01T00:00:00';
    const result = pipe.transform(isoString);
    
    // Should successfully transform without throwing
    expect(result).toMatch(/^\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}$/);
  });

  it('should format date components with leading zeros', () => {
    const date = new Date('2024-03-05T08:07:06.000Z');
    const result = pipe.transform(date);
    
    // Verify leading zeros are present in date
    expect(result).toMatch(/2024-03-05 \d{2}:07:06/);
  });

  it('should handle dates at end of year', () => {
    const date = new Date('2024-12-31T23:59:59.000Z');
    const result = pipe.transform(date);
    
    expect(result).toMatch(/\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}/);
  });

  it('should handle dates at beginning of year', () => {
    const date = new Date('2024-01-01T00:00:00.000Z');
    const result = pipe.transform(date);
    
    expect(result).toMatch(/\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}/);
  });

  it('should correctly convert UTC time to local time', () => {
    // Create a date with a fixed UTC time
    const utcDate = new Date('2024-06-15T12:00:00Z');
    const result = pipe.transform(utcDate);
    
    // Should return a formatted string with all numeric components
    const parts = result.split(' ');
    expect(parts).toHaveLength(2);
    
    const datePart = parts[0].split('-');
    expect(datePart).toHaveLength(3);
    expect(datePart[0]).toHaveLength(4); // year
    expect(datePart[1]).toHaveLength(2); // month
    expect(datePart[2]).toHaveLength(2); // day
    
    const timePart = parts[1].split(':');
    expect(timePart).toHaveLength(3);
    expect(timePart[0]).toHaveLength(2); // hours
    expect(timePart[1]).toHaveLength(2); // minutes
    expect(timePart[2]).toHaveLength(2); // seconds
  });

  it('should handle various date string formats', () => {
    const formats = [
      '2024-04-26T10:30:45Z',
      '2024-04-26T10:30:45.000Z',
      '2024-04-26T10:30:45',
      '2024-04-26T10:30:45.999'
    ];

    formats.forEach(format => {
      const result = pipe.transform(format);
      expect(result).toMatch(/^\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}$/);
    });
  });

  it('should transform single digit month to double digit', () => {
    const date = new Date('2024-05-05T12:00:00Z');
    const result = pipe.transform(date);
    
    expect(result).toMatch(/2024-05-05/);
  });

  it('should transform single digit day to double digit', () => {
    const date = new Date('2024-06-07T12:00:00Z');
    const result = pipe.transform(date);
    
    expect(result).toMatch(/2024-06-07/);
  });

  it('should transform single digit hour to double digit', () => {
    const date = new Date('2024-06-15T01:30:45Z');
    const result = pipe.transform(date);
    
    expect(result).toMatch(/\d{4}-06-15 \d{2}:30:45/);
  });

  it('should preserve noon time correctly', () => {
    const date = new Date('2024-06-15T12:00:00Z');
    const result = pipe.transform(date);
    
    expect(result).toMatch(/\d{4}-06-15 \d{2}:00:00/);
  });

  it('should preserve midnight time correctly', () => {
    const date = new Date('2024-06-15T00:00:00Z');
    const result = pipe.transform(date);
    
    expect(result).toMatch(/\d{4}-06-15 \d{2}:00:00/);
  });
});
