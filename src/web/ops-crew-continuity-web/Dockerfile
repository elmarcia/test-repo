FROM node:24-alpine AS build
WORKDIR /app

COPY src/web/ops-crew-continuity-web/package*.json ./
RUN npm install

COPY src/web/ops-crew-continuity-web/ ./
RUN npm run build

FROM nginx:1.27-alpine AS runtime
COPY src/web/ops-crew-continuity-web/nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=build /app/dist/ops-crew-continuity-web/browser /usr/share/nginx/html
EXPOSE 80
