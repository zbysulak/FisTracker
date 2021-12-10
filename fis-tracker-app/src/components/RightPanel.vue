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
        <span class="subheading mr-1">Time Left: </span>
        <b>{{ timeNeeded }}</b>
      </p>
      <p class="mb-1">
        <span class="subheading mr-1">Average Time </span>
        <b>{{ timeNeeded }}</b>
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
import ZImageUpload from './ZImageUpload.vue'
export default {
  name: "RightPanel",
  props: { time: { type: Object, require: true } },
  components: {ZImageUpload},

  computed: {
    loading() {
      return this.time == undefined || this.time.totalTime == undefined
    },

    totalTimeInMonth() {
      if (this.time != undefined && this.time.totalTime != undefined) {
        return (this.time?.timeNeeded?.totalHours || 0)
      } else {
        return "jeste nic"
      }
    },

    totalTime() {
      if (this.time != undefined && this.time.totalTime != undefined) {
        return this.time?.totalTime?.days * 24 + this.time?.totalTime?.hours  + ":" + this.time?.totalTime?.minutes.toString().padStart(2, '0')
      } else {
        return "jeste nic"
      }
    },

    timeNeeded() {
      if (this.time != undefined && this.time.totalTime != undefined) {
        return ( (this.time?.timeNeeded?.totalHours || 0) - this.time.totalTime.totalHours) + ":" + this.time?.timeNeeded?.minutes.toString().padStart(2, '0')
      } else {
        return "jeste nic"
      }
    }
  }
}
</script>
