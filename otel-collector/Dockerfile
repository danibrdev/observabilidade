FROM alpine:latest
RUN apk add --no-cache curl
# Copie o binário do Collector da imagem original
COPY --from=otel/opentelemetry-collector-contrib:latest /otelcol-contrib /otelcol-contrib
ENTRYPOINT ["/otelcol-contrib"]
CMD ["--config=/etc/otel-collector-config.yaml"]