const Agent = require('agentkeepalive');

module.exports = {
        '/api': {
            target: 'http://localhost:26464/',
            secure: false,
            agent: new Agent({
                maxSockets: 100,
                keepAlive: true,
                maxFreeSockets: 10,
                keepAliveMsecs: 1000,
                timeout: 6000,
                keepAliveTimeout: 9000
            }),
            onProxyRes: proxyRes => {
                let key = 'www-authenticate';
                proxyRes.headers[key] = proxyRes.headers[key] &&
                    proxyRes.headers[key].split(',');
            }
        }
};
