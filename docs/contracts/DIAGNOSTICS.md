# Diagnostics and error routing contract

Every operation carries `OperationId`, `FeatureId`, `ErrorCode`, `CorrelationId`, human message, technical message, host/document context, and relevant asset/element IDs. Exceptions are logged only and are never exposed as the primary user message.

Error prefixes are owned by features: `LIB`, `SEL`, `REP`, `KIT`, `ROM`, `REL`, `EXP`, `CUS`, `SHR`, `SET`, `DIA`, `TST`, and `INS`. A report routes first to the owning feature and correlation ID. Cross-feature code must not convert an error into an unrelated prefix.

The Test & Feedback Center creates one consolidated feedback package per stage containing environment, runtime/content versions, diagnostics, reproduction steps, previews, logs and sanitized project context.
