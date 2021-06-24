const withPlugins = require('next-compose-plugins');

const pwa = require('next-pwa');
const bundleAnalyzer = require('@next/bundle-analyzer')({
  enabled: process.env.ANALYZE === 'true',
});

const runtimeCaching = require('next-pwa/cache');

module.exports = withPlugins([
  [
    {
    }
  ],
  [{}
    // pwa, {
    //   pwa: {
    //     dest: 'public',
    //     runtimeCaching,
    //     disable: process.env.NODE_ENV === 'development',
    //     publicExcludes: [],
    //   },
    // },
  ],
  [
    bundleAnalyzer, {}
  ],
]);
