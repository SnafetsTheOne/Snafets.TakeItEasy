#!/usr/bin/env sh
set -eu

# Render env.js from template using env vars
envsubst < /usr/share/nginx/html/env.template.js > /usr/share/nginx/html/env.js

exec nginx -g "daemon off;"