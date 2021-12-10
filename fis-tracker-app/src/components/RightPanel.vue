<template>
  <div>
    <v-card-title>Month Overview</v-card-title>
    <v-skeleton-loader
      v-if="loading"
      class="mx-auto"
      max-width="300"
      type="card"></v-skeleton-loader>
    <v-card v-else>
      <v-card-text>
        <p class="mb-1">
          <span class="subheading mr-1">Month: </span>
          <b>{{ totalTimeInMonth }}</b>
          <span> hours</span>
        </p>
        <p class="mb-1">
          <span class="subheading mr-1">Worked: </span>
          <b>{{ totalTime }}</b>
        </p>
        <p class="mb-1">
          <span class="subheading mr-1">
            Remaining time to work this month:
          </span>
          <b>{{ remainingTimeNeeded }}</b>
        </p>
        <p class="mb-1">
          <span class="subheading mr-1">
            Average time you have to work for rest of this month:
          </span>
          <b>{{ averageTimeNeeded }}</b>
        </p>
      </v-card-text>
    </v-card>
    <v-card-title>Screenshot upload</v-card-title>
    <v-card>
      <z-image-upload />
    </v-card>
  </div>
</template>

<script>
import ZImageUpload from "./ZImageUpload.vue"

function formatTimeSpan(ts) {
  return ts.days * 24 + ts.hours + ":" + ts.minutes.toString().padStart(2, "0")
}
export default {
  name: "RightPanel",
  props: {
    time: { type: Object, require: true },
    loading: { type: Boolean, default: false }
  },
  components: { ZImageUpload },

  computed: {
    // number of working days in month * 8
    totalTimeInMonth() {
      console.log(this.time)
      if (this.time != undefined) {
        return formatTimeSpan(this.time.totalTimeNeeded)
      } else {
        return "jeste nic"
      }
    },
    // time worked this month
    totalTime() {
      if (this.time != undefined) {
        return formatTimeSpan(this.time.totalTimeWorked)
      } else {
        return "jeste nic"
      }
    },

    // remaining hours needed
    remainingTimeNeeded() {
      if (this.time != undefined) {
        return formatTimeSpan(this.time.remainingTimeNeeded)
      } else {
        return "jeste nic"
      }
    },

    averageTimeNeeded() {
      if (this.time != undefined) {
        return formatTimeSpan(this.time.averageTimeNeeded)
      } else {
        return "jeste nic"
      }
    }
  }
}
</script>
