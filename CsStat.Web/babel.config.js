module.exports = {
  presets: [
    ['@babel/preset-env', {
      useBuiltIns: 'usage',
    }],
    '@babel/preset-react',
    '@babel/preset-typescript'
  ],
  plugins: [
    '@babel/plugin-transform-object-assign',
    '@babel/plugin-proposal-class-properties',
    '@babel/plugin-syntax-dynamic-import',
    '@babel/plugin-proposal-object-rest-spread'
  ],
  env: {
    production: {
      plugins: [
        [
          'transform-react-remove-prop-types',
          {
            mode: 'remove',
            removeImport: true,
            ignoreFilenames: [
              'node_modules'
            ]
          }
        ]
      ]
    }
  }
};
