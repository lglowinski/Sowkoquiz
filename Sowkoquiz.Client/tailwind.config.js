/** @type {import('tailwindcss').Config} */
module.exports = {
  purge: ['./**/*.html', './**/*.razor'],
  darkMode: false, // or 'media' or 'class'
  theme: {
    extend: {
      colors: {
        'gray-100': '#f7fafc',
        'gray-50': '#f9fafb',
        'gray-200': '#edf2f7',
        'gray-800': '#2d3748',
        'gray-600': '#718096',
      },
    },
  },
  variants: {
    extend: {},
  },
  plugins: [
    require('@tailwindcss/forms'),
  ],
}