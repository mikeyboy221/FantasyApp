/** @type {import('tailwindcss').Config} */

module.exports = {
  mode: 'jit',
  content: [
    './Areas/**/*.cshtml',
    './Pages/**/*.cshtml',
    './Views/**/*.cshtml',
    './node_modules/flowbite/**/*.js'
  ],
  theme: {
    extend: {},
  },
  plugins: [
    require('flowbite/plugin')
  ]
}


