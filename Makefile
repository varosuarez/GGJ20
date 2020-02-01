SHELL := bash
.SHELLFLAGS := -eu -o pipefail -c
.ONESHELL:
.DELETE_ON_ERROR:
MAKEFLAGS += --warn-undefined-variables
MAKEFLAGS += --no-builtin-rules


all: Builds/.web_itch

Builds/.web_itch: Builds/.web
	$$BUTLER push Builds/Web alvarber/a-bot-2-repair:web

Builds/.web: $(shell find Assets -type f | sed 's: :\\ :g') Packages/manifest.json
	"$$UNITY" \
		-batchmode \
		-executeMethod HCF.Build.BuildWeb \
		-logFile /dev/stdout \
		-quit
