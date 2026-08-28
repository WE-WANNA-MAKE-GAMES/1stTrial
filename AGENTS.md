<!-- BEGIN:unity-agent-rules -->

# This is NOT the C# you know

## Sensitive files

The following files must never be accessed or inspected by the agent:

- `.env.local`

Rules:
- Do not read these files.
- Do not inspect their contents.
- Do not modify them.
- Do not copy, print, summarize, or expose their contents.
- Do not include their values in code, logs, patches, commits, or responses.
- If information from these files is required, ask the user to provide only the specific non-sensitive value needed.

<!-- END:nextjs-agent-rules -->
