# Enterprise .NET 10 Observability & CQRS Suite

![NuGet Version](https://img.shields.io/badge/NuGet-1.0.0-blue.svg)
![Target Framework](https://img.shields.io/badge/.NET-10.0-purple.svg)
![Architecture](https://img.shields.io/badge/Architecture-CQRS-blue.svg)
![Build Status](https://img.shields.io/badge/build-passing-brightgreen.svg)

**Author:** Meet Shah  
**System:** .NET 10, CQRS, MediatR, Serilog, Seq, PostgreSQL   
**Repository:** [Enterprise-DotNet-Observability-Suite](https://github.com/MeetShah/Enterprise-DotNet-Observability-Suite)

## The Problem It Solves
Most mid-market enterprise APIs suffer from silent failures, bloated monolithic architectures, and fragmented logging. When a critical production error occurs, debugging takes days instead of minutes.

## The Solution
This repository contains a production-ready boilerplate bridging **.NET 10 (CQRS Pattern)** with centralized, high-performance observability. It is designed to be dropped into existing architectures to immediately provide enterprise-grade tracking and error taxonomy.

### Core Capabilities:
* **Opinionated Zero-Config Logging:** Connects automatically to Seq and PostgreSQL via `appsettings.json`.
* **CQRS Auto-Logging:** MediatR pipeline behaviors automatically wrap Commands/Queries to log execution times and outcomes without polluting business logic.
* **Cross-Service Tracing:** Automatically extracts and propagates `X-Correlation-ID` headers across the distributed system.
* **Global Error Taxonomy:** Standardized JSON error responses mapped to specific domain exception codes, preventing stack-trace leaks to the client.
* **Alerting:** Designed to hook into MS Teams/Slack webhooks for fatal system crashes.

### 💼 Need this implemented for your team?
I specialize in custom implementations of this architecture for US/EU startups and mid-market firms. If your .NET infrastructure needs modernizing, or you are migrating to a Next.js/React frontend and need a robust backend API, let's talk.

**https://www.linkedin.com/in/me-shah-meet/**
