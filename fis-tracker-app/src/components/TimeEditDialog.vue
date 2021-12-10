<template>
  <div>
    <v-btn color="primary" dark class="mb-2" @click="newEntry">
      New entry
    </v-btn>
    <v-dialog v-model="dialog" max-width="500px">
      <v-card>
        <v-card-title>
          <span class="text-h5">{{ formTitle }}</span>
        </v-card-title>
        <v-card-text>
          <v-container>
            <v-row>
              <v-col cols="12" sm="6" md="12">
                <date-picker-input
                  v-model="entry.date"
                  label="Date"></date-picker-input>
              </v-col>
              <v-col cols="12" sm="6" md="6">
                <time-picker-input v-model="entry.in" label="Arrived at">
                </time-picker-input>
              </v-col>
              <v-col cols="12" sm="6" md="6">
                <time-picker-input
                  v-model="entry.out"
                  label="Left at"
                  clearable>
                </time-picker-input>
              </v-col>
              <v-expand-transition>
                <v-col cols="2" class="ma-0" md="12" v-show="lunch">
                  <v-row>
                    <v-col cols="12" sm="6" md="6">
                      <time-picker-input
                        v-model="entry.lunchOut"
                        label="Lunch break start"
                        clearable>
                      </time-picker-input>
                    </v-col>
                    <v-col cols="12" sm="6" md="6">
                      <time-picker-input
                        v-model="entry.lunchIn"
                        label="Lunch break end"
                        clearable>
                      </time-picker-input>
                    </v-col>
                  </v-row>
                </v-col>
              </v-expand-transition>
              <v-col cols="12" sm="6" md="6">
                <v-checkbox v-model="lunch" label="Lunch break"></v-checkbox>
              </v-col>
              <v-col cols="12" sm="6" md="6">
                <v-checkbox
                  v-model="entry.homeOffice"
                  label="Homeoffice / Holiday"></v-checkbox>
              </v-col>
            </v-row>
          </v-container>
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="blue darken-1" text @click="close"> Cancel </v-btn>
          <v-btn color="blue darken-1" text @click="save"> Save </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>
<script>
import axios from "axios"
import TimePickerInput from "./TimePickerInput.vue"
import DatePickerInput from "./DatePickerInput.vue"
export default {
  components: { TimePickerInput, DatePickerInput },
  props: { value: { type: Object, require: true } },
  computed: {
    formTitle() {
      return this.editedIndex === -1 ? "New entry" : "Edit entry"
    },
    isNewEntry() {
      return this.editedIndex === -1
    }
  },
  data: () => ({
    defaultEntry: {
      id: -1,
      date: new Date().toISOString().substring(0, 10),
      in: null,
      out: null,
      lunchIn: null,
      lunchOut: null,
      homeOffice: false
    },
    lunch: true,
    editingId: -1,
    dialog: false,
    entry: {}
  }),
  watch: {
    value(n) {
      n.in = n.in?.formatted
      n.out = n.out?.formatted
      n.lunchOut = n.lunchOut?.formatted
      n.lunchIn = n.lunchIn?.formatted
      this.entry = n
      this.dialog = true
    },
    dialog(v) {
      if (v) {
        const settings = JSON.parse(window.localStorage.settings)
        this.lunch = !!settings.defaultLunch
      }
    }
  },
  methods: {
    newEntry() {
      this.value = Object.assign({}, this.defaultEntry)
      this.dialog = true
    },
    close() {
      console.log("CLOSE MODAL")
      this.dialog = false
    },
    save() {
      axios
        .post(this.appConfig.apiUrl + "/TimeInputs", this.entry, {
          withCredentials: true
        })
        .then(() => {
          this.$emit("saved", this.editingId === -1 ? "new" : "edit")
        })
        .catch((e) => console.error(e))
        .finally(() => this.close())
    }
  }
}
</script>
