import { loadEnv } from 'vite'
import { defineConfig } from 'vitest/config'
import { svelte } from '@sveltejs/vite-plugin-svelte'
import * as path from 'path'

// https://vitejs.dev/config/
export default defineConfig({
  test: {
    globals: true, // required
    setupFiles: ['vitest-localstorage-mock'],
    mockReset: false,
  },
  plugins: [svelte()],
  envDir: '.',
  resolve: {
    alias: {
      '@src': path.resolve(__dirname, './src'),
      '@mocks': path.resolve(__dirname, './mocks'),
      '@test': path.resolve(__dirname, './test'),
      '@public': path.resolve(__dirname, './public'),
    }
  },
  //https://github.com/material-svelte/vite-web-test-runner-plugin/issues/4
  //https://github.com/vitejs/vite/issues/2579#issuecomment-841770842
  optimizeDeps: {
    include: [
      // '@testing-library/svelte',
      // 'chai',
      // 'chai-dom',
      // 'testing-library___dom',
      // '@testing-library/user-event',
      // 'sinon',
      'lodash.get',
      'lodash.isequal',
      'lodash.clonedeep'
    ]
  },
  server: {
    port: 3000
  },
})
