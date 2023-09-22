import vite from 'vite-web-test-runner-plugin'
import {playwrightLauncher} from '@web/test-runner-playwright'
import {defaultReporter} from '@web/test-runner'
import {junitReporter} from '@web/test-runner-junit-reporter'

export default {
  plugins: [vite()],
  coverageConfig: {
    include: ['src/**/*.{svelte,js,jsx,ts,tsx}']
  },
  browsers: [playwrightLauncher({product: 'chromium'})],
  reporters: [
    defaultReporter({reportTestResults: true, reportTestProgress: true}),
    junitReporter()
  ],
  testRunnerHtml: (testFramework) => `
  <!DOCTYPE html>
    <html>
      <head>
        <script type="module">
          window.global = window;
          window.process = { env: { ENV: 'TEST' } };
        </script>
        <script type="module" src="/test/hooks.ts"></script>
        <script type="module" src="${testFramework}"></script>
      </head>
    </html>
  `
}
