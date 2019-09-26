import 'antd/dist/antd.css';
import './general/scss/normalize.css';
import './general/scss/index.scss';

import './components/home-page';
import './components/players-data';
import './components/player-card';

/* init app */
import app from './general/ts/app.ts';

app.init();

/* require svg */
const files = require.context('./general/svg', true, /^\.\/.*\.svg/);
files.keys().forEach(files);

// do not focus sprite in IE
const spriteNode = document.getElementById('__SVG_SPRITE_NODE__');

if (spriteNode) {
    spriteNode.setAttribute('focusable', 'false');
    spriteNode.setAttribute('aria-hidden', 'true');
}
