FROM alpine:3.22.1

# Install curl for health checks and debugging
RUN apk add --no-cache curl

# Download the otelcol-contrib binary for v0.134.0
ADD https://github.com/open-telemetry/opentelemetry-collector-releases/releases/download/v0.134.0/otelcol-contrib-linux-amd64 /otelcol-contrib

# Make the binary executable
RUN chmod +x /otelcol-contrib

# Set the entrypoint to run the collector
ENTRYPOINT ["/otelcol-contrib"]